import speech_recognition as sr
import pyttsx3
import os
import docx
import PyPDF2
import re
import requests


# === НАСТРОЙКИ ===
DOCUMENTS_FOLDER = "C:/Users/79186/PycharmProjects/BOT/doc"
HF_API_TOKEN = "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"  # Вставьте ваш токен от Hugging Face
HF_API_URL = "https://api-inference.huggingface.co/models/mistralai/Mistral-7B-Instruct-v0.1"  # Корректный URL
CHUNK_SIZE = 5000

# === ГОЛОСОВОЙ ВЫВОД ===
engine = pyttsx3.init()
def speak(text):
    engine.say(text)
    engine.runAndWait()

# === РАСПОЗНАВАНИЕ РЕЧИ ===
def recognize_speech():
    recognizer = sr.Recognizer()
    with sr.Microphone() as source:
        print("Говорите...")
        recognizer.adjust_for_ambient_noise(source)
        audio = recognizer.listen(source)
    try:
        return recognizer.recognize_google(audio, language="ru-RU")
    except sr.UnknownValueError:
        return ""
    except sr.RequestError:
        return ""

# === ЗАГРУЗКА ФАЙЛОВ ===
all_chunks = []

def split_text_into_chunks(text, chunk_size=CHUNK_SIZE):
    return [text[i:i + chunk_size] for i in range(0, len(text), chunk_size)]

def load_all_files():
    global all_chunks
    if not os.path.exists(DOCUMENTS_FOLDER):
        os.makedirs(DOCUMENTS_FOLDER)
        print(f"Создана папка: {DOCUMENTS_FOLDER}")
    print(f"Проверяю папку: {DOCUMENTS_FOLDER}")
    files = os.listdir(DOCUMENTS_FOLDER)
    print(f"Найдено файлов: {len(files)}")
    if not files:
        print("Папка пуста!")
        return

    for filename in files:
        file_path = os.path.join(DOCUMENTS_FOLDER, filename)
        ext = filename.split('.')[-1].lower()
        text = ""
        print(f"Обрабатываю файл: {filename}")

        try:
            if ext == "txt":
                with open(file_path, "r", encoding="utf-8") as file:
                    text = file.read()
            elif ext == "docx":
                doc = docx.Document(file_path)
                text = " ".join([para.text for para in doc.paragraphs])
            elif ext == "pdf":
                with open(file_path, "rb") as file:
                    reader = PyPDF2.PdfReader(file)
                    text = " ".join([page.extract_text() for page in reader.pages if page.extract_text()])
            else:
                print(f"Формат файла {filename} не поддерживается")
                continue
        except Exception as e:
            print(f"Ошибка чтения {filename}: {e}")
            continue

        if not text:
            print(f"Текст в файле {filename} не извлечён")
            continue

        chunks = split_text_into_chunks(text)
        for i, chunk in enumerate(chunks):
            all_chunks.append({"file": filename, "chunk": chunk, "index": i})
        print(f"Загружен файл: {filename}, чанков: {len(chunks)}, длина текста: {len(text)} символов")

# === ПОИСК РЕЛЕВАНТНЫХ ЧАНКОВ ===
def find_relevant_chunks(query, top_n=1):
    query_lower = query.lower()
    if "первая" in query_lower or "начало" in query_lower or "опиши первую" in query_lower:
        relevant = sorted([item for item in all_chunks], key=lambda x: x["index"])[:top_n]
        print(f"Выбраны первые чанки: {[item['index'] for item in relevant]}")
        return relevant

    query_words = set(re.findall(r'\w+', query_lower))
    important_terms = {"нлп", "nlp"} & query_words
    similarities = []

    for item in all_chunks:
        chunk_lower = item["chunk"].lower()
        score = sum(1 for word in query_words if word in chunk_lower)
        if important_terms:
            score += sum(10 if term in chunk_lower else 0 for term in important_terms)
        similarities.append((score, item))

    similarities.sort(reverse=True, key=lambda x: x[0])
    relevant = [item for score, item in similarities[:top_n] if score > 0]
    print(f"Найдено релевантных чанков: {len(relevant)} с максимальным score: {similarities[0][0] if similarities else 0}")
    return relevant if relevant else [sorted(all_chunks, key=lambda x: x["index"])[0]]

# === HUGGING FACE INFERENCE API ===
def query_hf_api(user_query):
    relevant_items = find_relevant_chunks(user_query)
    context = "\n".join([item["chunk"] for item in relevant_items])
    sources = list(set([item["file"] for item in relevant_items]))

    # Форматируем промпт в стиле инструкций для Mistral
    full_prompt = (
        "[INST] Ты — научный помощник. Отвечай строго по содержимому предоставленного текста на русском языке. "
        "Если спрашивают про первую главу или начало, кратко перескажи содержание текста. "
        "Если спрашивают 'что такое', дай определение из текста или скажи 'Термин не найден в тексте'. "
        "Не добавляй лишних комментариев.\n\n"
        f"Текст:\n{context}\n\n"
        f"Вопрос: {user_query}\nОтвет: [/INST]"
    )

    print(f"Отправляемый контекст (длина: {len(context)} символов): {context[:100]}...")

    headers = {
        "Authorization": f"Bearer {HF_API_TOKEN}",
        "Content-Type": "application/json"
    }
    payload = {
        "inputs": full_prompt,
        "parameters": {
            "max_new_tokens": 300,
            "temperature": 0.5,
            "top_p": 0.9,
            "do_sample": True
        }
    }

    try:
        response = requests.post(HF_API_URL, headers=headers, json=payload)
        response.raise_for_status()  # Проверка на ошибки HTTP
        result = response.json()
        if isinstance(result, list) and len(result) > 0:
            generated_text = result[0].get("generated_text", "").strip()
            # Убираем промпт из ответа, если он включён
            if generated_text.startswith(full_prompt):
                generated_text = generated_text[len(full_prompt):].strip()
            return generated_text, sources
        else:
            return "Не удалось получить ответ от API", sources
    except requests.exceptions.RequestException as e:
        print(f"Ошибка API Hugging Face: {e}")
        return "Не удалось обработать запрос", sources

# === ГЛАВНЫЙ ЦИКЛ ===
def main():
    speak("Привет, я ваш научный ассистент. Загружаю документы...")
    print("Запуск загрузки документов...")
    load_all_files()
    if not all_chunks:
        speak("Документы не найдены. Проверьте папку.")
        print("Список чанков пуст!")
        return

    speak(f"Загружено {len(all_chunks)} чанков из научных работ. Задавайте вопросы!")
    print(f"Всего загружено чанков: {len(all_chunks)}")

    while True:
        command = recognize_speech().lower()
        print("Вы сказали:", command)
        if not command:
            continue
        if "выход" in command or "стоп" in command:
            speak("До свидания!")
            break
        else:
            speak("Обрабатываю запрос...")
            response, sources = query_hf_api(command)
            speak(response)
            if sources:
                speak(f"Информация из файлов: {', '.join(sources)}")
            print("Ответ:", response)
            print("Источники:", sources)

if __name__ == "__main__":
    main()
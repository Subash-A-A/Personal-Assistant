import whisper


def speech_to_text(audio, model="base"):
    model = whisper.load_model(model)
    result = model.transcribe(audio)
    return result["text"]

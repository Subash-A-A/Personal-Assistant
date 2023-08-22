import voice
import asyncio
from flask import Flask, request, jsonify, make_response
from translate import en_to_japanese, lang_to_en
from gpt import get_completion
from speech_to_text import speech_to_text
from voice_recorder import VoiceRecorder

app = Flask(__name__)
recorder = VoiceRecorder()


@app.route('/status', methods=['GET'])
def status():
    return jsonify(status="ok")


@app.route('/record', methods=['GET'])
def record_audio():
    recorder.record_handler()
    return jsonify(recording_status=recorder.recording)


@app.route('/speak', methods=['POST'])
def post_data():
    # Retrieve the JSON data from the request body
    data = request.get_json()
    lang = data.get('srcLang')
    speakerID = data.get('speakerID')
    streamingAssetsPath = data.get('streamingAssetsPath')

    # Convert user recording to text
    raw_message = speech_to_text("recording.wav")
    message = lang_to_en(raw_message, src=lang)

    # Generate reply to user's question
    gpt_answer = get_completion(message)

    # Translate response to japanese
    jp_text = en_to_japanese(gpt_answer["answer_en"])
    expression = gpt_answer["tone"]
    asyncio.run(voice.speak(jp_text, streamingAssetsPath, speaker=speakerID))

    # Response
    response = make_response()

    # Set the response headers
    response.headers.set('Expression', expression)
    response.headers.set('Prompt', message)
    response.headers.set('Reply', gpt_answer["answer_en"])

    # Return the voice file as a response
    return response


@app.route('/speak_text', methods=['POST'])
def post_data_text():
    # Retrieve the JSON data from the request body
    data = request.get_json()
    lang = data.get('srcLang')
    speakerID = data.get('speakerID')
    streamingAssetsPath = data.get('streamingAssetsPath')
    textInput = data.get('message')

    # Convert user text from any language to english
    message = lang_to_en(textInput, src=lang)

    # Generate reply to user's question
    gpt_answer = get_completion(message)

    # Translate response to japanese
    jp_text = en_to_japanese(gpt_answer["answer_en"])
    expression = gpt_answer["tone"]
    asyncio.run(voice.speak(jp_text, streamingAssetsPath, speaker=speakerID))

    # Response
    response = make_response()

    # Set the response headers
    response.headers.set('Expression', expression)
    response.headers.set('Prompt', message)
    response.headers.set('Reply', gpt_answer["answer_en"])

    # Return the voice file as a response
    return response


if __name__ == '__main__':
    app.run()

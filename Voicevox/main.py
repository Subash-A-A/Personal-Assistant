import voice
import asyncio
from flask import Flask, request, jsonify, send_file, make_response
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

    # Get the message and synthesize the audio
    # message = data.get('message')
    raw_message = speech_to_text("recording.wav")
    lang = data.get('srcLang')
    message = lang_to_en(raw_message, src=lang)

    speakerID = data.get('speakerID')
    streamingAssetsPath = data.get('streamingAssetsPath')

    gpt_answer = get_completion(message)

    print(f"Message: {message}, Response: {gpt_answer} SpeakerID: {speakerID}")

    jp_text = en_to_japanese(gpt_answer["answer_en"])
    expression = gpt_answer["tone"]
    asyncio.run(voice.speak(jp_text, streamingAssetsPath, speaker=speakerID))

    file_path = f"{streamingAssetsPath}/voice.wav"

    # Read the voice file
    voice_data = open(file_path, 'rb')

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

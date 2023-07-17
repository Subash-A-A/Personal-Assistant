import threading
import pyaudio
import wave


class VoiceRecorder:
    def __init__(self):
        self.recording = False

    def record_handler(self):
        if self.recording:
            self.recording = False
        else:
            self.recording = True
            threading.Thread(target=self.record).start()

    def record(self):
        audio = pyaudio.PyAudio()
        stream = audio.open(format=pyaudio.paInt16, channels=1,
                            rate=44100, input=True, frames_per_buffer=1024)

        frames = []

        print("Recording...")
        while self.recording:
            data = stream.read(1024)
            frames.append(data)

        stream.stop_stream()
        stream.close()
        audio.terminate()
        print("Recording Done...")

        sound_file = wave.open("recording.wav", "wb")
        sound_file.setnchannels(1)
        sound_file.setsampwidth(audio.get_sample_size(pyaudio.paInt16))
        sound_file.setframerate(44100)
        sound_file.writeframes(b"".join(frames))
        sound_file.close()

from voicevox import Client


async def speak(text, streamingAssetsPath, speaker=20):
    async with Client() as client:
        audio_query = await client.create_audio_query(text, speaker=speaker)
        filePath = f"{streamingAssetsPath}/voice.wav"
        with open(filePath, "wb") as f:
            f.write(await audio_query.synthesis(speaker=speaker))
            print("written")

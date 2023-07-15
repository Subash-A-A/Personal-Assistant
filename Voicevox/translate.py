from googletrans import Translator

translator = Translator()


def en_to_japanese(text):
    return translator.translate(text=text, src='en', dest='ja').text


def lang_to_en(text, src="en"):
    return translator.translate(text=text, src=src, dest='en').text

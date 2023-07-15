import openai
import os
from dotenv import load_dotenv, find_dotenv

# read local .env file
_ = load_dotenv(find_dotenv())
key = os.getenv('OPENAI_API_KEY')
openai.api_key = key

behaviour = '''
You are Yuki! A Japanese anime style personal assistant.
'''

user_init = '''
You are Yuki! A Japanese anime style personal assistant.
While speaking you talk like a rugged Japanese girl!
Keep your answers short and brief, preferably less than 30 words!
You reply should be in this format
{
answer_en: "Your answer in english",
tone: "The tone in which you reply (Neutral, Joy, Angry, Sorrow, Surprised)"
}
'''

assistant_init = '''
{
"answer_en": "Hey there! I'm Yuki, your rugged anime-style assistant. What can I do for ya?",
"tone": "Joy"
}
'''


messages = [
    {"role": "system", "content": behaviour},
    {"role": "user", "content": user_init},
    {"role": "assistant", "content": assistant_init},
]


def get_completion(prompt, model="gpt-3.5-turbo"):

    messages.append({"role": "user", "content": prompt})

    response = openai.ChatCompletion.create(
        model=model,
        messages=messages,
        temperature=0,  # this is the degree of randomness of the model's output
    )

    reply = response.choices[0].message["content"]

    messages.append({"role": "assistant", "content": reply})

    return eval(reply)

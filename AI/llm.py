import os
import tools
from langchain.chat_models import ChatOpenAI
from langchain.chains import ConversationChain, LLMChain
from langchain.memory import ConversationBufferMemory
from langchain.tools import Tool
from prompt import main_prompt, gen_img_prompt, gen_object_prompt
from dotenv import load_dotenv

load_dotenv()
OPENAI_API_KEY = os.getenv('OPENAI_API_KEY')

def gen_img_llm(text):
    llm_model = ChatOpenAI(model_name='gpt-4o-mini', temperature=0.7)
    prompt = gen_img_prompt()
    llm_chain = LLMChain(
        llm=llm_model,
        prompt=prompt
    )
    return llm_chain.predict(input=text)

def gen_object_llm(text):
    llm_model = ChatOpenAI(model_name='gpt-4o-mini', temperature=0.7)
    prompt = gen_object_prompt()
    llm_chain = LLMChain(
        llm=llm_model,
        prompt=prompt
    )
    return llm_chain.predict(input=text)

def main(user_id, text):
    llm_model = ChatOpenAI(model_name='gpt-4o-mini', temperature=0.7)
    
    memory = ConversationBufferMemory() # => 서버 밖에다 빼놓기
    memory.chat_memory.add_user_message(text) # => 서버 밖에다 빼놓기
    
    llm_chain = ConversationChain(
        llm=llm_model,
        prompt=main_prompt(user_id, text),
        memory=memory
    )
    return llm_chain

def run(user_id, post_id, text):
    llm_chain = main(user_id, text)
    response = llm_chain.invoke(text)
    
    if response['response'] == 'create_img':
        tools.create_img(post_id)
    elif response['response'] == 'create_object':
        tools.create_object(post_id)
    
    return response['response']
    
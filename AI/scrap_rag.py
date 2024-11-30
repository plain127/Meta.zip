import json
import os
import numpy as np
from lightrag import LightRAG, QueryParam
from lightrag.llm import gpt_4o_mini_complete, gpt_4o_complete
from lightrag.utils import EmbeddingFunc
from langchain.embeddings import OpenAIEmbeddings
from langchain.embeddings.base import Embeddings
from dotenv import load_dotenv

load_dotenv()
OPENAI_API_KEY = os.getenv('OPENAI_API_KEY')

def parsing(json_path):
    with open(json_path, 'r', encoding='utf-8') as f:
        contents = json.load(f)
    
    post = contents['posts'][0]
    page = post['pages'][0]
    element = page['elements'][0]
    return element['content']
    

def create_txt(content):
    file_name = 'temp.txt'
    with open(file_name, 'w', encoding='utf-8') as f:
        f.write(content)

def delete_txt():
    os.remove('temp.txt')

async def embedding_func(texts: list[str]) -> np.ndarray:
    embeddings = OpenAIEmbeddings(model = 'text-embedding-ada-002').embed_documents(texts)
    return np.array(embeddings)
    
def main(user_id, json_path):
    text_content = parsing(json_path)
    create_txt(text_content)
    
    if not os.path.exists(f'./{user_id}db'):
        os.mkdir(f'./{user_id}db')
        
    rag = LightRAG(
        working_dir=f'./{user_id}db',
        llm_model_func=gpt_4o_mini_complete,
        #embedding_func=EmbeddingFunc(
        #embedding_dim=1536,
        #max_token_size=8192,
        #func=embedding_func,
    #)
    )
    
    with open('temp.txt', encoding='utf-8') as f:
        rag.insert(f.read())
        
    delete_txt()

def return_text_rag(user_id):
    rag = LightRAG(
        working_dir=f'./{user_id}db',
        llm_model_func=gpt_4o_mini_complete
    )
    
    return rag

import os
import base64
from langchain_core.messages import SystemMessage, HumanMessage
from langchain.chat_models import ChatOpenAI
from langchain.embeddings import OpenAIEmbeddings
from lightrag import LightRAG
from lightrag.llm import gpt_4o_mini_complete
from lightrag.utils import EmbeddingFunc
from scrap_rag import embedding_func, delete_txt
from dotenv import load_dotenv

load_dotenv()
OPENAI_API_KEY = os.getenv('OPENAI_API_KEY')

def ocr_screenshot(user_id, post_id): #바이너리 파일 그대로 받는다면 매개변수 수정
    with open(f'./{post_id}.png', 'rb') as f:
        image_bytes = base64.b64decode(f.read()).decode('utf-8')
     
    chat = ChatOpenAI(model='gpt-4o-mini')
    msg = chat.invoke(
        [
            SystemMessage(content='''
                          You are a helpful assistant that can answer questions about image.
                          Use the following pieces of retrieved image to answer the question.
                          You only say about this image.
                          '''),
            HumanMessage(
                content=[
                    {'type':'text', 'text':'전체 내용을 말해'},
                    {
                        'type':'image_url',
                        'image_url':{'url': f'data:image/png;base64,{image_bytes}'},
                    }
                ]
            )
        ]
    )
    
    file_name = 'temp.txt'
    with open(file_name, 'w', encoding='utf-8') as f:
        f.write(msg.content)

    rag = LightRAG(
        working_dir=f'./{user_id}db',
        llm_model_func=gpt_4o_mini_complete,
        embedding_func=EmbeddingFunc(
        embedding_dim=1536,
        max_token_size=8192,
        func=embedding_func,
    )
    )
    
    with open('temp.txt') as f:
        rag.insert(f.read())
        
    delete_txt
    
    print(msg.content)

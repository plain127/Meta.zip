# Meta.zip
# 프로젝트 소개
![image](https://github.com/user-attachments/assets/6ac5a283-4df7-4313-a6bb-f9ac00cff21b)
![image](https://github.com/user-attachments/assets/5b74f744-1bb9-45a4-bd73-ac3e01d97345)
- 웹진 기사를 AI가 생성한 엔티티로 나만의 스크랩 북을 3D 공간에 만드는 플랫폼

# 목적

- 글자를 분위기로 바꾸는 프로젝트
- 웹진 + 멀티모달 AI + 메타버스

# AI 기능

- A* 알고리즘 사용한 AI NPC가 캐릭터를 따라다님
- LLM 사용 채팅
- 다국어 번역
- RAG 사용 기사 업데이트 및 저장 관리
- RAG 기반 사용자가 관심 있는 분야 추천
- OCR 이용 기사 스크린 캡처 저장
- 기사 커버 이미지 생성 (기사 썸네일)
- 기사의 핵심 소재 3D 오브젝트 생성 (3D 오브젝트를 누르면 스크랩한 기사가 나옴)
- Function Calling : 자연어 명령으로 이미지 생성, 3D 오브젝트 생성

# LLM

- **Model : gpt-4o-mini**

# RAG

- **LightRAG**
    - SOTA(2024.09 ~ 2024.11 )
    - Embedding
        - **Model : text-embedding-ada-002**
        - OpenAIEmbeddings
        - Dimention : 1536
        - max_token_size : 8196
    - VectorDB : nano-VectorDB
- Tools : Langchain
- Prompt
    - 한국어
    - LightRAG의 retriever 반영
    - llm 프롬프트
        - name_prompt : NPC 캐릭터 이름과 말투 설정
        - role_prompt : 기사 작업 관련 역할 부여
        - classify_prompt : function calling 역할 부여
    - 이미지 생성 프롬프트
    - 3D 생성 프롬프트
- Memory
    - ConversationBufferMemory
- Chain
    - ConversationChain

# OCR

- **Model : gpt-4o-mini**
- png, jpeg 이미지를 바이너리 코드로 변환해 이를 모델에 넘겨주어 사용

# Function Calling

- **Model : gpt-4o-mini**
- Langchain의 Tools 기능 사용

# Text to Image

- **Model**
    - VAE
    - Unet
    - **FLUX.1-dev (기사 썸네일 생성)**
    - **SD1.5 (3D 오브젝트 생성을 위한)**
- Workflow
    - FLUX.1-dev
        
        ![image](https://github.com/user-attachments/assets/420e590c-0ad8-409c-970b-d0e8ec1c0059)
        
    - SD1.5
      
        ![image](https://github.com/user-attachments/assets/366fe377-a36b-4674-85a5-e0af86ac9b64)
        
- LoRA
    - magazine_cover2.safetensors
    - 3dzujianV3-00010.safetensors

# Image to 3D

- **Model : TripoSR**
- Workflow
    
![image](https://github.com/user-attachments/assets/0b4ce297-8e0c-4d8c-a87e-baa44710a5af)
    

# Symbolic AI

- Tools : Unity NavMeshAgent

# UI/UX 클라이언트

- Tool : Unity
  
# Server

- 멀티플레이 : Photon, Firebase
- AI : Nginx, Docker, Uvicorn, FastAPI
- 기사 스크랩 및 호출 : PostgreSQL, Qdrant

# 내가 맡은 작업

- 모든 AI 개발
- C# HTTP통신 코드 작성

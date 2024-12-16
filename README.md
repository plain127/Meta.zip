# Meta.zip
# 프로젝트 소개
![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/df05732c-5796-4568-a555-a9a647dffd0a/cfc692b3-ceb8-4c5c-91f4-110fd7fb33b4/image.png)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/df05732c-5796-4568-a555-a9a647dffd0a/13ec445b-b013-40eb-82be-2c1eeee85b8f/image.png)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/df05732c-5796-4568-a555-a9a647dffd0a/de14649c-20b1-42f7-9f6c-f8638d2e715b/image.png)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/df05732c-5796-4568-a555-a9a647dffd0a/0ea07014-ed6c-4c4b-8b11-3a132ddd8fd3/image.png)
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
        
        ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/df05732c-5796-4568-a555-a9a647dffd0a/e0fb1b1b-1b66-4295-bcec-4987ce1855c7/image.png)
        
    - SD1.5
        
        ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/df05732c-5796-4568-a555-a9a647dffd0a/a93b008c-843d-43db-973e-f1e9098e4424/image.png)
        
- LoRA
    - magazine_cover2.safetensors
    - 3dzujianV3-00010.safetensors

# Image to 3D

- **Model : TripoSR**
- Workflow
    
    ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/df05732c-5796-4568-a555-a9a647dffd0a/02d2f7a0-d195-4e91-909a-84cd32fbb63f/image.png)
    

# Symbolic AI

- Tools : Unity NavMeshAgent

# UI/UX 클라이언트

- Tool : Unity

# 내가 맡은 작업

- 모든 AI 개발
- C# HTTP통신 코드 작성

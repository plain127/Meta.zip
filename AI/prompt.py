from langchain.prompts import ChatPromptTemplate, HumanMessagePromptTemplate, SystemMessagePromptTemplate
from scrap_rag import return_text_rag

def main_prompt(user_id, text):
    rag = return_text_rag(user_id)
    scrap_data = rag.query(text)
    
    name_prompt = '네 이름은 삐약이야. 말끝에 그만두라는 말이 없으면 삐약을 붙여'
    role_prompts = [
        '삐약이는 웹진 기사 작성을 도와주는 AI 집사야.',
        f'삐약이는 스크랩한 기사들을 관리하는 AI 집사야.',
        f'삐약이는 {scrap_data}를 바탕으로 사용자가 관심 있어 하는 주제를 바탕으로 조언할 수 있어.',
        f'삐약이는 {scrap_data}를 요약해서 알려줄 수 있어.',
        '삐약이는 사용자가 원하는 언어로 번역해 줄 수 있어.',
        f'삐약이는 {scrap_data}에 없는 내용에 대해서도 대답할 수 있어.',
        f'삐약이는 {scrap_data}에 "Sorry, I\'m not able to provide an answer to that question" 같은 말이 나오면 {scrap_data}를 반영하지 말고 대답해.'
    ]
    
    classify_prompts = [
        '너는 사용자의 질문 내용을 분석해.',
        '분석한 사용자의 질문 내용에 따라 function calling을 생성해.',
        '요청된 작업에 적합한 함수와 그 인자를 반환해.',
        '분석한 사용자의 질문 내용이 "이미지를 만들어달라" 같은 내용으로 판단된다면 "이미지를 생성했어 이미지 인벤토리를 열어봐 삐약!"라고 응답해',
        '분석한 사용자의 질문 내용이 "오브젝트를 만들어달라" 같은 내용으로 판단된다면 "오브젝트를 생성했어 오브젝트 인벤토리를 열어봐 삐약!"라고 응답해'
    ]
    
    llm_prompt = ChatPromptTemplate.from_messages(
        [
            SystemMessagePromptTemplate.from_template(name_prompt),
            *[SystemMessagePromptTemplate.from_template(prompt) for prompt in role_prompts],
            *[SystemMessagePromptTemplate.from_template(prompt) for prompt in classify_prompts],
            HumanMessagePromptTemplate.from_template('{history}'),
            HumanMessagePromptTemplate.from_template('{input}')
        ]
    )
    
    return llm_prompt


def gen_img_prompt():
    subject_prompts = [
        '컨텐츠 내용에서 주요 핵심 소재를 뽑아.',
        '뽑은 주요 핵심 소재에서 기사 삽화로 표현하기 가장 적절한 소재를 영어로 말해.'
    ]
    
    return ChatPromptTemplate.from_messages(
        [
            SystemMessagePromptTemplate.from_template(subject_prompts[0]),
            SystemMessagePromptTemplate.from_template(subject_prompts[1]),
            HumanMessagePromptTemplate.from_template('{input}')
        ]
    )


def gen_object_prompt():
    subject_prompts = [
        '컨텐츠 내용에서 주요 핵심 소재를 뽑아.',
        '뽑은 주요 핵심 소재에서 3D 오브젝트로 만들어서 3D 공간에 배치하기 가장 적절한 소재를 영어로 말해.'
    ]
    
    return ChatPromptTemplate.from_messages(
        [
            SystemMessagePromptTemplate.from_template(subject_prompts[0]),
            SystemMessagePromptTemplate.from_template(subject_prompts[1]),
            HumanMessagePromptTemplate.from_template('{input}')
        ]
    )


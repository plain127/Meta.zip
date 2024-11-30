import json
import gen_cover_img as gi
import gen_object as go

def parsing(json_path):
    with open(json_path, 'r', encoding='utf-8') as f:
        contents = json.load(f)
    
    post = contents['posts'][0]
    page = post['pages'][0]
    element = page['elements'][0]
    return element['content']

def create_img(post_id):
    import llm
    text = parsing(f'./{post_id}.json')
    context = llm.gen_img_llm(text)
    gi.make_magazine_cover(post_id, context)
    response = "이미지를 만들었습니다! 삐약"
    return response

def create_object(post_id):
    import llm
    text = parsing(f'./{post_id}.json')
    context = llm.gen_object_llm(text)
    go.make_object(post_id, context)
    response = "3D 오브젝트를 만들었습니다! 삐약"
    return response

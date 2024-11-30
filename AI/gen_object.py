import websocket
import uuid
import json
import urllib.request
import urllib.parse
import comfy_connect
import os
import shutil

cf = comfy_connect

def make_object(id, text):
    output_folder = f'./{id}/object'
    source_folder = 'ComfyUI/output'
    
    with open('object_api.json', 'r', encoding='utf-8') as f:
        prompt_text = f.read()

    prompt = json.loads(prompt_text)
    prompt['31']['inputs']['text'] = '3D cartoon-style' + text
    prompt['14']['inputs']['save_path'] = f'{id}.obj'

    ws = websocket.WebSocket()
    ws.connect('ws://{}/ws?clientId={}'.format(cf.server_address, cf.client_id))
    images = cf.get_images(ws, prompt)
    ws.close()

    file_extensions = ['.obj', '.png']
    
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    for filename in os.listdir(source_folder):
        if any(filename.endswith(ext) for ext in file_extensions):
            source_path = os.path.join(source_folder, filename)
            output_path = os.path.join(output_folder, filename)
            shutil.move(source_path, output_path)

import socket
import json
import time
from _thread import *

joins = dict()
evt_queue = list()

def make_data(**datas):
    return datas

def evt_thread():
    while True:
        while len(evt_queue) > 0:
            evt = evt_queue.pop(0)
            # print(evt)
            if evt['evt'] == 'join':
                ''
            elif evt['evt'] == 'quit':
                if evt['id'] in joins.keys():
                    try:
                        joins[evt['id']].close()
                    except Exception:
                        ''
                    del joins[evt['id']]
            elif evt['evt'] == 'update':
                if evt['id'] in joins.keys():
                    joins[evt['id']]['x'] = evt['x']
                    joins[evt['id']]['y'] = evt['y']
                    joins[evt['id']]['angle'] = evt['angle']
        for id, player in joins.items():
            players = []
            for id2, player2 in joins.items():
                if id != id2:
                    players.append(make_data(id=id2, hp=player2['hp'], x=player2['x'], y=player2['y'], angle=player2['angle']))
            player['sock'].send(str(json.dumps(make_data(evt='update', hp=player['hp'], others=players)) + "#").encode())
        time.sleep(0.05)

def threaded(client_socket, addr, id):
    print('client connected: ', addr[0], ':', addr[1], sep='')
    recv_data = ""
    while True:
        try:
            tmp = client_socket.recv(1024).decode()
            recv_data += tmp
            if not recv_data:
                evt_queue.append(make_data(evt='quit', id=id))
                break
            split_data = recv_data.split('#')
            if recv_data[-1] != '#':
                recv_data = split_data[-1]
                split_data = split_data[:-1]
            else:
                recv_data = ""

            for data in split_data:
                if data == "":
                    continue
                jsonData = json.loads(data)
                keys = jsonData.keys()
                if jsonData['evt'] == 'join':
                    if 'nickname' in keys and 'color' in keys:
                        client_socket.send(json.dumps(make_data(result='success')).encode())
                        evt_queue.append(make_data(evt='join', id=id))
                        nickname = jsonData['nickname']
                        color = jsonData['color']
                        x = 0.0
                        y = 0.0
                        angle = 0.0
                        joins[id] = make_data(nickname=nickname, color=color, hp=100, x=x, y=y, angle=angle, sock=client_socket)
                    else:
                        client_socket.send(json.dumps(make_data(result='fail')).encode())
                if jsonData['evt'] == 'update':
                    if 'x' in keys and 'y' in keys and 'angle' in keys:
                        x = jsonData['x']
                        y = jsonData['y']
                        angle = jsonData['angle']
                        evt_queue.append(make_data(evt='update', id=id, x=x, y=y, angle=angle))

        except ConnectionResetError:
            evt_queue.append(make_data(evt='quit', id=id))
            break

        except socket.timeout:
            evt_queue.append(make_data(evt='quit', id=id))
            break

        except Exception as e:
            print('client error: ', addr[0], ':', addr[1], ' : ', e, sep='')

    client_socket.close()

HOST = ''
PORT = 9172

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind((HOST, PORT))
server_socket.listen()

print('server start')

socket.setdefaulttimeout(1)
start_new_thread(evt_thread, ())

id = 0
while True:
    client_socket, addr = server_socket.accept()
    start_new_thread(threaded, (client_socket, addr, id))
    id += 1

server_socket.close()
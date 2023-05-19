import socket
import json
import time
import threading
from _thread import *

joins = dict()
evt_queue = list()
lock = threading.Lock()

def make_data(**datas):
    return datas

def evt_thread():
    while True:
        begin_time = time.time()
        while len(evt_queue) > 0:
            evt = evt_queue.pop(0)
            # print(evt)
            if evt['evt'] == 'join':
                print(evt)
                lock.acquire()
                lock.release()
            elif evt['evt'] == 'quit':
                print(evt)
                lock.acquire()
                if evt['id'] in joins.keys():
                    try:
                        joins[evt['id']].close()
                    except Exception:
                        ''
                    del joins[evt['id']]
                lock.release()
            elif evt['evt'] == 'update':
                if evt['id'] in joins.keys():
                    joins[evt['id']]['x'] = evt['x']
                    joins[evt['id']]['y'] = evt['y']
                    joins[evt['id']]['angle'] = evt['angle']
        items = list(joins.items())
        for id, player in items:
            players = []
            try:
                for id2, player2 in items:
                    if id != id2:
                        players.append(make_data(id=id2, hp=player2['hp'], x=round(player2['x'], 2), y=round(player2['y'], 2), angle=round(player2['angle'], 2)))
                message = str(json.dumps(make_data(evt='update', hp=player['hp'], others=players)) + "#")
                player['sock'].send(message.encode())
            except Exception:
                ''
        end_time = time.time()
        wait_time = 0.2 - (end_time - begin_time)
        if wait_time > 0:
            time.sleep(wait_time)

def threaded(client_socket, addr, id):
    print('client connected: ', addr[0], ':', addr[1], ' with id ', id, sep='')
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
                if jsonData['evt'] == 'join':
                    lock.acquire()
                    nickname = jsonData['nickname']
                    color = jsonData['color']
                    x = 0.0
                    y = 0.0
                    angle = 0.0
                    hp = 100
                    mapp = []
                    others = []
                    for id0, other in joins.items():
                        others.append(make_data(id=id0, nickname=other['nickname'], color=other['color'], x=round(other['x'], 2), y=round(other['y'], 2), angle=round(other['angle'], 2), hp=other['hp']))
                    client_socket.send(str(json.dumps(make_data(id=id, nickname=nickname, color=color, x=x, y=y, hp=hp, map=mapp, others=others)) + '#').encode())
                    evt_queue.append(make_data(evt='join', id=id, nickname=nickname, color=color, hp=hp, x=x, y=y, angle=angle))
                    joins[id] = make_data(nickname=nickname, color=color, hp=hp, x=x, y=y, angle=angle, sock=client_socket)
                    print('join', id)
                    lock.release()

                if jsonData['evt'] == 'update':
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

socket.setdefaulttimeout(2)
start_new_thread(evt_thread, ())

global_id = 0
while True:
    client_socket, addr = server_socket.accept()
    start_new_thread(threaded, (client_socket, addr, global_id))
    global_id += 1

server_socket.close()

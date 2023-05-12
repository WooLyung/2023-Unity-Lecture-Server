import socket
import json
import time
from _thread import *

joins = dict()
event_queue = list()

def make_data(**datas):
    return datas

def event_thread():
    while True:
        while len(event_queue) > 0:
            evt = event_queue.pop(0)
            if evt['event'] == 'join':
                print('join event')
            elif evt['event'] == 'quit':
                del joins[evt['id']]
        for id, player in joins.items():
            print(id, player)
        time.sleep(0.05)

def threaded(client_socket, addr, id):
    print('client connected: ', addr[0], ':', addr[1], sep='')
    client_socket.settimeout(1)
    while True:
        try:
            data = client_socket.recv(1024)
            if not data:
                # print('client disconnected: ', addr[0], ':', addr[1], sep='')
                event_queue.append(make_data(event='quit', id=id))
                break
            # print('Received from ' + addr[0], ':', addr[1], data.decode())
            jsonData = json.loads(data.decode())
            if jsonData['event'] == 'join':
                if 'nickname' in jsonData.keys() and 'color' in jsonData.keys():
                    client_socket.send(json.dumps(make_data(result='success')).encode())
                    event_queue.append(make_data(event='join', id=id))
                    nickname = jsonData['nickname']
                    color = jsonData['color']
                    joins[id] = make_data(nickname=nickname, color=color, hp=100, sock=client_socket)
                else:
                    client_socket.send(json.dumps(make_data(result='fail')).encode())

        except ConnectionResetError as e:
            # print('client disconnected: ', addr[0], ':', addr[1], sep='')
            event_queue.append(make_data(event='quit', id=id))
            break

        except socket.timeout:
            event_queue.append(make_data(event='quit', id=id))
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

start_new_thread(event_thread, ())

id = 0
while True:
    client_socket, addr = server_socket.accept()
    start_new_thread(threaded, (client_socket, addr, id))
    id += 1

server_socket.close()
import socket
from _thread import *

def threaded(client_socket, addr):
    print('client connected: ', addr[0], ':', addr[1], sep='')
    while True:
        try:
            data = client_socket.recv(1024)
            if not data:
                print('client disconnected: ', addr[0], ':', addr[1], sep='')
                break
            print('Received from ' + addr[0], ':', addr[1], data.decode())
            # client_socket.send(data)
        except ConnectionResetError as e:
            print('client disconnected: ', addr[0], ':', addr[1], sep='')
            break
    client_socket.close()

HOST = '127.0.0.1'
PORT = 9172

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind((HOST, PORT))
server_socket.listen()

print('server start')
while True:
    client_socket, addr = server_socket.accept()
    start_new_thread(threaded(client_socket, addr))

server_socket.close()
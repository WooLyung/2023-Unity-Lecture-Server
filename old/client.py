import socket

HOST = '34.22.79.114'
PORT = 9172

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((HOST, PORT))
sock.sendall('asdf'.encode())

data = sock.recv(1024)
print(data.decode())

sock.close()
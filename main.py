import socket

s = socket.socket()
host = socket.gethostname()
port = 1025

s.bind((host, port))
s.listen(5)

print("Server started!")

while True:
    c, addr = s.accept()
    print('joined', addr)
    c.close()
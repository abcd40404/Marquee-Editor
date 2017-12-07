#!/usr/bin/env python
#-*- coding:utf-8 -*-　

from binascii import unhexlify
from binascii import hexlify
import time
import serial
import paho.mqtt.client as mqtt

Topic = "A1"
MosquittoIP = "localhost"

# ser = serial.Serial(
#     port='/dev/ttyAMA0',
#     baudrate=9600,
#     parity=serial.PARITY_NONE,
#     stopbits=serial.STOPBITS_ONE,
#     bytesize=serial.EIGHTBITS,
#     timeout=1
# )

# str.encode/hexlify 將字串轉為指定編碼字串
# unhexlify 將16進位字串轉為16進位數據

def get_checksum(args):
    res = 0
    for i in args:
        num = int(i, 16)
        res = res ^ num
    return "{:02x}".format(res)

def Data_toHex(Station, Type, Data):
    args = [] # 存放16進位字串參數
    for c in Station:
        args.append(hexlify(c))
    args.append(Type)
    res = ''
    if(Type == 'B1' or Type == 'B2'):
        #行號 停留時間 前功能 文字模式碼 屬性 文字 後功能
        result = Data.split(',')
        if(Type == 'B1'):
            Lines = result[0] # int 1~254
            args.append("{:02x}".format(int(Lines)))
        elif(Type == 'B2'):
            Times = reslut[0] # int 1 ~ 255
            args.append("{:02x}".format(int(Times)))
        StayTime = result[1] # int 0~255
        PreFunc = result[2] # single char
        Text_type = result[3] # 已經是 Hex(C0, C1)
        Attribute = result[4] # 已經是 Hex
        Text = result[5]
        PostFunc = result[6] # single char
        args.append("{:02x}".format(int(StayTime)))
        args.append(hexlify(PreFunc))
        args.append(Text_type[:2])
        # 英文半形, 中文全形
        Text = Text.decode('utf-8') # utf-8 轉為 unicode字串

        if(Text_type == 'C00'): # 半形
            for i in range(0, 10):
                args.append(Attribute)
                args.append(hexlify(Text[i]))
        elif(Text_type == 'C01'): # 全形
            for i in range(0, 5):
                if(u'\u4e00' <= Text[i] <= u'\u9fff'): # 中文 utf8 範圍
                    args.append(Attribute)
                    args.append(Attribute)
                    args.append(hexlify(Text[i].encode('big5')))
        elif(Text_type == 'C1'): # 上下兩行的半形
            for i in range(0, 20):
                args.append(Attribute)
                args.append(hexlify(Text[i]))
        args.append(hexlify(PostFunc))
        checksum = get_checksum(args) # 計算 checksum
        # 轉為16進位數據
        for i in args:
            res = res + unhexlify(i)
        res = res + unhexlify(checksum)
    elif(Type == 'B6' or Type == 'B7'):
        result = Data.split(',')
        Num = int(result[0]) # int
        args.append("{:02x}".format(Num))
        for i in range(1, Num+1):
            args.append("{:02x}".format(int(result[i])))

        checksum = get_checksum(args)
        # 轉為16進位數據
        for i in args:
            res = res + unhexlify(i)
        res = res + unhexlify(checksum)

    elif(Type == 'B8'): #顯示行號
        result = Data.split(',')
        Start = result[0] # int 1~255
        End = result[1] # int 1~255
        args.append("{:02x}".format(int(Start)))
        args.append("{:02x}".format(int(End)))
        checksum = get_checksum(args) # 計算 checksum
        for i in args:
            res = res + unhexlify(i)
        res = res + unhexlify(checksum)
    else:
        a=1

    return res

def on_connect(client, userdata, flags, rc):
    print("Connected with result code "+str(rc))
    client.subscribe(Topic)

def on_message(client, userdata, msg):
    result = msg.payload.split("|")
    Station = result[0]
    Type = result[1] #已經是 Hex
    Data = result[2]
    Date = result[3]
    res = '\x02' + Data_toHex(Station, Type, Data) + '\x03'
    print repr(res)

    # print(msg.topic+" "+msg.payload.decode('utf-8').encode('big5'))
    #my_string = "\x02\x41\x31\xB8\x01\x12\xDB\x03"
    my_string = "\x02\x41\x31\xB1\x03\x05\x41\xC0\x01\x48\x01\x45\x01\x4C\x01\x4C\x01\x4F\x01\x57\x01\x4F\x01\x52\x01\x4C\x01\x44\x41\x07\x03"
    # print repr(my_string)
    # ser.write(res)

client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.connect(MosquittoIP, 1883, 60)
client.loop_forever()

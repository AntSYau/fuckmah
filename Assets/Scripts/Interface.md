## 报文格式
### client
Player_Name;Room_ID;request_type;argument1;argument2;....;
### server
request_type;argument1;argument2;....;



## 进入游戏后


#### 指定各自东南西北座位
东 dir_1
南 dir_2
西 dir_3
北 dir_4

``` python
# Field
dir_1=username
# Example
;dir_1=username;
;dir_2=username;
;dir_3=username;
;dir_4=username;
```

#### 指定一个庄家， 并且广播

``` python
# Field
zhuang
# Example
;zhuang=dong;
;zhuang=xi;
;zhuang=nan;
;zhuang=bei;
```
#### 一开始投两个骰子， 并且广播结果

``` python
# Field
touzi
# Example
;touzi=(1,2);
```

#### 初始化牌堆

#### 发牌

```
# Server (to one player)
ResponseType=RoomInit;Dir_1=username;Dir_2=username;Dir_3=username;Dir_4=username;Zhuang=1;Dice1=3;Dice2=4;HandCardList=11,12,13,16,17,18,24,25,26,37,38,39,43,46;
# User
UserName=zhuangjia;RoomID=123;RequestType=OutCard;OutCard=46;
# Server (broadcast)
ResponseType=OutCard;OutCard=46;
# User
UserName=qiushi;RoomID=123;RequestType=Intension;Peng=0;Chi=1;Gang=0;Hu=0;
## Peng, Chi, Gang
UserName=qiushi;RoomID=123;RequestType=Desition;Action=1;
### Server (broadcast)
ResponseType=Desition;User=3;Operation=Chi;
## Hu
UserName=qiushi;RoomID=123;RequestType=Desition;Action=1;Score=10;
## Do not take action
UserName=qiushi;RoomID=123;RequestType=Desition;Action=0;
# Server (fapai)
ResponseType=InCard;InCard=11;
# 
UserName=qiushi;RoomID=123;Peng=0;Chi=1;Gang=0;Hu=0;
UserName=qiushi;RoomID=123;RequestType=Desition;Action=1;
# Server (guangbo)
# no one has action:
ResponseType=NoAction;
# else:
ResponseType=ZiMoDesition;User=3;Action=2;Card=11;
Username=qiushi;RoomID=123;OutCard=11;
# gameover
ResponseType=GameOver;Winner=1;Score=10;
ResponseType=Draw;
```

#### 指定时间给某一玩家发牌
``` python
# Field
pai
# Example
;pai=dong;
```
## 牌名
万 1
一万到九万 11-19
条 2
一条到九条 21-29
筒 3
一筒到九筒 31-39
字 4
东西南北中发白 41-47

``` python
# Field
hand_card_list
# Example
;hand_card_list=11,12,14,21;
```


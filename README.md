# MultiplayerShooterExample

  #      CursorVisible ESC

#	MultiplayerManager инитит клиента , подключатеся к комнате, и обрабатывает евенты о изменеии состояния комнаты, + узнает RTT  : 
# основные поля -

# 		public float RTT { get; private set; }
 #		public Action<Player> OnCreatePlayerLocal;
#		public Action<Player> OnCreateEnemy;
 #  		public Action<Player> OnRemoveEnemy;
 #               public Action<ShootingInfo> ShootingEnemyEvent;

#	 NetworkEntitySpawner спавнит / уничтожает плееера и врага, хранит список врагов на сцене 

#	 InputController (синглтон ) для удобства доступен из любого места в часности PlayerCharacter

# PlayerLocal: 

#	 PlayerMovementModel хранит актуальные данные о перемещении (UniRX)

#        SittingState  подписывается на Реактивные свойства в PlayerMovementModel и при изменении садится или встает(UniRX), аналогично у врага

#       PlayerAnimatorController подписывается на Реактивные свойства в PlayerMovementModel и при изменении воспроизводит нужную анимацию(UniRX), аналогично у врага

#	 PlayerCharacter двигает плеера и сетит данные position / velosity и другие данны связанные с перемещением в PlayerMovementModel 

# ⦁	 PlayerMovementSyncer берет данные из PlayerMovementModel, экстраполирует отсительно пигна и отправляет их на сервер 
# (экстраполяция происходит чтобы на момент когда данные дойдут до сервера они были приблизительно актуальными, дальнейшую экстраполяцию 
# уже производит принимающий клиент относительно его пигна)


# Enemy:

#	 EnemyController принимает данные с сервера и сетит их в PlayerMovementModel

#	 EnemyMovementController берет данные из PlayerMovementModel, экстраполирует отсительно пигна и двигает обьект 

#        EnemyShootingController подписывается на событие выстрела плеера из MultiplayerManager и стреляет пустышкой (данные о пуле приходят через Action)


# Weapon: (пока кривовато, хочется пограмотнее организовать оружие, сделаю когда будем делать смену оружия и разные пули ) 

#       ShootingController занимается непостредственно выстрелом из текущего оружия и передает данные на сервер о пуле.

#	 WeaponBase абстрактный класс для оружия, реализует базовую логику, TryShoot / патроны и перезарядку 

#	 Pistol реализует логику базового класса

#        Остальное не особо важно.

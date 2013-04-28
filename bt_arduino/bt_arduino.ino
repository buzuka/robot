// Bluetooth controlled platform
#include <SoftwareSerial.h>   //Software Serial Port

// Pins
#define RxD 2
#define TxD 3

// Magic
#define DEBUG_ENABLED  1

// Helpers
typedef enum {
  OFF = 0,
  STARTING,
  RUNNING,
  STOPPING
} DriveState;

typedef struct {
  DriveState state;
  int dirPin;
  int powerPin;
  long cmdBegin;
  long cmdRun;
  long cmdStopping;
  long cmdEnd;
  int baseValue;
  int currentValue;
} DriveData;

/*********** Globals ************/
// bluetooth port
SoftwareSerial blueToothSerial(RxD,TxD);

//Standard PWM DC control
int E1 = 5;     //M1 Speed Control
int E2 = 6;     //M2 Speed Control
int M1 = 4;    //M1 Direction Control
int M2 = 7;    //M2 Direction Control

// Hardware keys
int adc_key_val[5] = {30, 150, 360, 535, 760};
int NUM_KEYS = 5; 

// Control structures
// Operand's timeout
int TIMEOUT = 1000;
int cmd = -1;
int op1 = -1;
int op2 = -1;
int op3 = -1;
int op4 = -1;
long lastCheck = 0;

DriveData left;
DriveData right;

/*********** End globals ************/

/**************** Commands *******************/
void doCmd(void * dd, int forward, int value) {
  DriveData* s = (DriveData*)dd;
  analogWrite(s->powerPin, value);
  digitalWrite(s->dirPin, forward);

  Serial.println(forward);
  Serial.println(value);
  
  // remember all
  s->currentValue = value;
}

void setup(void) 
{ 
  left.state = OFF;
  left.dirPin = M1;
  left.powerPin = E1;
  right.state = OFF;
  right.dirPin = M2;
  right.powerPin = E2;
  
  pinMode(E1, OUTPUT);  
  pinMode(E2, OUTPUT);  
  pinMode(M1, OUTPUT);  
  pinMode(M2, OUTPUT);  
  
  pinMode(RxD, INPUT);
  pinMode(TxD, OUTPUT);

  // led on
  pinMode(13, OUTPUT);  
  digitalWrite(13, HIGH);
  
  Serial.begin(19200);      //Set Baud Rate
  
  setupBlueToothConnection();
  Serial.println("Run keyboard control");
}

// from bluetooth shield's sample
void setupBlueToothConnection()
{
  blueToothSerial.begin(38400);
  blueToothSerial.print("\r\n+STWMOD=0\r\n"); //set the bluetooth work in slave mode
  blueToothSerial.print("\r\n+STNA=SeeedBTSlave\r\n"); //set the bluetooth name as "SeeedBTSlave"
  blueToothSerial.print("\r\n+STOAUT=1\r\n"); // Permit Paired device to connect me
  blueToothSerial.print("\r\n+STAUTO=0\r\n"); // Auto-connection should be forbidden here
  delay(2000); // This delay is required.
  blueToothSerial.print("\r\n+INQ=1\r\n"); //make the slave bluetooth inquirable 
  Serial.println("The slave bluetooth is inquirable!");
  delay(2000); // This delay is required.
  blueToothSerial.flush();
}

void loop(void) 
{
  // Main loop here
  // Get current time
  long now = millis();
  if (cmd != -1) {
    if ((now - lastCheck) > TIMEOUT) {
      // Reset command, wait next one
      lastCheck = now;
      cmd = -1;
      op1 = -1;
      op2 = -1;
      op3 = -1;
      op4 = -1;
      Serial.println("Command reset");
    }
  }
  // Check bluetooth data
  if (blueToothSerial.available()) {
    int c = blueToothSerial.read();
    bool ready = false;
    if (cmd == -1) {
      cmd = c;
    }
    else if (op1 == -1) {
      op1 = c;
    }
    else if (op2 == -1) {
      op2 = c;
    }
    else if (op3 == -1) {
      op3 = c;
    }
    else if (op4 == -1) {
      op4 = c;
      ready = true;
    }
    lastCheck = now;
    if (ready) {
      Serial.println("Got command");
      switch (cmd) {
        case 'L':
          left.cmdBegin = now;
          // TODO: implement starting and stopping time marks
          left.cmdRun = now + 0;
          left.cmdStopping = now + op3 * 10;
          left.cmdEnd = left.cmdStopping;
          // set power
          left.baseValue = op1;
          doCmd(&left, op1 & 0x1, op1 & 0xFE);
          left.state = RUNNING;
          Serial.print("Command ");
          Serial.print((char)cmd);
          Serial.print(" for ");
          Serial.print(op3 * 10);
          Serial.println(" ms");
          break;
        case 'R':
          right.cmdBegin = now;
          // TODO: implement starting and stopping time marks
          right.cmdRun = now + 0;
          right.cmdStopping = now + op3 * 10;
          right.cmdEnd = right.cmdStopping;
          // set power
          right.baseValue = op1;
          doCmd(&right, op1 & 0x1, op1 & 0xFE);
          right.state = RUNNING;
          Serial.print("Command ");
          Serial.print((char)cmd);
          Serial.print(" for ");
          Serial.print(op3 * 10);
          Serial.println(" ms");
          break;
      }
      cmd = -1;
      op1 = -1;
      op2 = -1;
      op3 = -1;
      op4 = -1;
    }
  }
  if (left.state != OFF) {
    if (left.cmdEnd < now) {
      // switch left off
      // TODO: implement starting and stopping
      doCmd(&left, false, 0);
      left.state = OFF;
      Serial.println("Left off");
    }
  }
  if (right.state != OFF) {
    if (right.cmdEnd < now) {
      // switch right off
      // TODO: implement starting and stopping
      doCmd(&right, false, 0);
      right.state = OFF;
      Serial.println("Right off");
    }
  }
}

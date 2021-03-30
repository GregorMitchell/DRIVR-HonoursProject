//https://www.instructables.com/How-to-Measure-Angle-With-MPU-6050GY-521/

#include<Wire.h>
#include "BluetoothSerial.h"

#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

BluetoothSerial SerialBT;

int IRSensor = 4;
int buttonPin = 5;
float length1 = 0.282;
float speed1 = 0.0;

bool readyStart = true;
bool readyStop = false;
bool earlyDetect = false;

unsigned long startMicros = 0;
unsigned long stopMicros = 0;
unsigned long elapsedMicros = 0;
bool initialReadingTaken = false;

float initialAngle = 0.0;
float hitAngle = 0.0;
float movedAngle = 0.0;

const int MPU_addr = 0x68;
int16_t AcX, AcY, AcZ, Tmp, GyX, GyY, GyZ;

int minVal = 265;
int maxVal = 402;

double x;
double y;
double z;

void setup()
{
  pinMode (IRSensor, INPUT);
  pinMode(buttonPin, INPUT_PULLUP);

  Wire.begin();
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);

  Serial.println("");

  Serial.begin(115200);
  Serial.println();

  SerialBT.begin("DRIVR");
}

void loop()
{
  int statusSensor = digitalRead (IRSensor);
  int buttonValue = digitalRead(buttonPin);


  Wire.beginTransmission(MPU_addr);
  Wire.write(0x3B);
  Wire.endTransmission(false);

  Wire.requestFrom(MPU_addr, 14, true);
  AcX = Wire.read() << 8 | Wire.read();
  AcY = Wire.read() << 8 | Wire.read();
  AcZ = Wire.read() << 8 | Wire.read();

  int xAng = map(AcX, minVal, maxVal, -90, 90);
  int yAng = map(AcY, minVal, maxVal, -90, 90);
  int zAng = map(AcZ, minVal, maxVal, -90, 90);

  x = RAD_TO_DEG * (atan2(-yAng, -zAng) + PI);
  y = RAD_TO_DEG * (atan2(-xAng, -zAng) + PI);
  z = RAD_TO_DEG * (atan2(-yAng, -xAng) + PI);

  if (buttonValue == LOW) {

    readyStart = true;
    readyStop = false;
    earlyDetect = false;

    startMicros = 0;
    stopMicros = 0;
    elapsedMicros = 0;

    initialAngle = y;
    Serial.print("initial Angle : ");
    Serial.println(initialAngle);
    
    hitAngle = 0.0;
    movedAngle = 0.0;

    Serial.println("Button Pressed");
  }

  if (statusSensor == 1)                                  //doesn't sense object
  {
    if (earlyDetect == false)
    {
      Serial.println("early detecting...finished!");
      earlyDetect = true;
    }

    if (readyStop == true && earlyDetect == true)
    {
      stopMicros = micros();
      readyStop = false;

      elapsedMicros = stopMicros - startMicros;

      speed1 = ((length1 / elapsedMicros) * 1000000) * 2.237;

      Serial.print(speed1);
      Serial.println(" mph");

      hitAngle = y;
      movedAngle = hitAngle - initialAngle;

      Serial.print("hit Angle : ");
      Serial.println(hitAngle);
      Serial.print("Moved Angle : ");
      Serial.println(movedAngle);
    }
  }
  else if (statusSensor == 0)                             //senses object
  {
    if (earlyDetect == false)
    {
      Serial.println("detecting...early detect");
    }

    if (readyStart == true && earlyDetect == true)
    {
      startMicros = micros();
      readyStop = true;
      readyStart = false;
    }
  }
}

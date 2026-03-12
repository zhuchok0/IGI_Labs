import math
import sys
import json

def CircleArea(r):
    return math.pi * r * r


def CirclePerimeter(r):
    return 2 * math.pi * r

def SquareArea(a):
    return a * a


def SquarePerimeter(a):
    return 4 * a

def CalculateFigures(config):
    print("\n" + "="*50)
    print("GEOMETRICAL CALCULATIONS")
    print("="*50)

    if 'circle' in config:
        r = config['circle'].get('radius', 5)
        area = CircleArea(r)
        perimeter = CirclePerimeter(r)
        print(f"\n CIRCLE:")
        print(f"   Radius: {r}")
        print(f"   Area: {area:.2f}")
        print(f"   Perimeter: {perimeter:.2f}")
    
    if 'square' in config:
        a = config['square'].get('side', 4)
        area = SquareArea(a)
        perimeter = SquarePerimeter(a)
        print(f"\n SQUARE:")
        print(f"   Side: {a}")
        print(f"   Area: {area:.2f}")
        print(f"   Perimeter: {perimeter:.2f}")
    
    print("\n" + "="*50)

config_path = 'config.json'  # значение по умолчанию
with open(config_path, 'r') as f:
    config = json.load(f)

CalculateFigures(config)
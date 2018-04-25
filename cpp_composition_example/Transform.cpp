#include "Transform.h"
	
void Transform::SetPosition(Point3 position) {
	_position = position;
}

Point3 Transform::GetPosition() { 
	return _position;
} 

#ifndef TRANSFORM_H
#define TRANSFORM_H

#include "Utilities.h"
#include "Component.h"

class Transform : public Component {
	Point3 _position;
	
	public:
		void SetPosition(Point3 position);
		Point3 GetPosition();
};

#endif
#ifndef ENTITY_H
#define ENTITY_H

#include <vector>
#include "Component.h"

class Entity {
    std::vector<Component*> components;

    public:
        ~Entity();
		
		void Update();
		
		void Destroy();
		
        template<class T>
        Component* AddComponent();
        
        template<class T>
        T* GetComponent();
}; 

#include "Entity_impl.h"

#endif
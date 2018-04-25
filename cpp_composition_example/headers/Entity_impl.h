/* Q: Why on earth would we place implementation into a .h file? Hint: Templates make things messy */

#ifndef ENTITY_IMPL_H
#define ENTITY_IMPL_H

template<class T>
Component* Entity::AddComponent() {
	Component* new_component = new T;
	components.push_back(new_component);
	new_component->Start();
	return new_component;
}
        
template<class T>
T* Entity::GetComponent() {
	int num_components = components.size();
	
	for(int i = 0; i < num_components; i++) {
		Component* c = components[i];
		T* casted_component = dynamic_cast<T*>(c);
		
		if(casted_component) {
			return casted_component;
		}
	}
	
	return nullptr;
}

#endif
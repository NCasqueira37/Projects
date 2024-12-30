#include "Vector2Int.h"

Vector2Int Vector2Int::operator+(Vector2Int other) {
	return Vector2Int(x + other.x, y + other.y);
}
Vector2Int Vector2Int::operator-(Vector2Int other) {
	return Vector2Int(x - other.x, y - other.y);
}
Vector2Int Vector2Int::operator*(Vector2Int other) {
	return Vector2Int(x * other.x, y * other.y);
}
Vector2Int Vector2Int::operator/(Vector2Int other) {
	return Vector2Int(x / other.x, y / other.y);
}

bool Vector2Int::operator=(Vector2Int other) {
	return(x == other.x && y == other.y);
}
bool Vector2Int::operator!=(Vector2Int other) {
	return(x != other.x && y != other.y);
}
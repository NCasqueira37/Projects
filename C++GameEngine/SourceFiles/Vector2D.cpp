#include "Vector2D.h"


Vector2D Vector2D::operator+(const Vector2D& other) const {
	return Vector2D(x + other.x, y + other.y);
}
Vector2D Vector2D::operator-(const Vector2D& other) const {
	return Vector2D(x - other.x, y - other.y);
}
Vector2D Vector2D::operator/(const Vector2D& other) const {
	return Vector2D(x / other.x, y / other.y);
}
Vector2D Vector2D::operator*(const Vector2D& other) const {
	return Vector2D(x * other.x, y * other.y);
}


Vector2D Vector2D::operator+(const double& amount) const {
	return Vector2D(x + amount, y + amount);
}
Vector2D Vector2D::operator-(const double& amount) const {
	return Vector2D(x - amount, y - amount);
}
Vector2D Vector2D::operator*(const double& amount) const {
	return Vector2D(x * amount, y * amount);
}
Vector2D Vector2D::operator/(const double& amount) const {
	return Vector2D(x / amount, y / amount);
}


bool Vector2D::operator=(const Vector2D& other) const {
	return (x == other.x && y == other.y);
}
bool Vector2D::operator!=(const Vector2D& other) const {
	return (x != other.x && y != other.y);
}
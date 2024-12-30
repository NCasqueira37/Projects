#pragma once

class Vector2Int {

public:
	int x, y;

	Vector2Int() :x(0), y(0) {};
	Vector2Int(int x, int y) :x(x), y(y) {};

	Vector2Int operator+(Vector2Int other);
	Vector2Int operator-(Vector2Int other);
	Vector2Int operator*(Vector2Int other);
	Vector2Int operator/(Vector2Int other);

	bool operator=(Vector2Int other);
	bool operator!=(Vector2Int other);
};
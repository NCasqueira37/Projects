#include <iostream>
#include "SDL.h"
#include "Vector2Int.h"

int main(int argc, char* argv[])
{
	std::string windowTitle = "SDL Window";
	int windowWidth = 800;
	int windowHeight = 600;

	bool running = true;
	SDL_Window* window = nullptr;
	SDL_Renderer* renderer = nullptr;

	if (SDL_Init(SDL_INIT_VIDEO) < 0) {
		std::cout << "Error: ";
		std::cout << SDL_GetError();
		SDL_Quit();
	}
	else {
		SDL_CreateWindowAndRenderer(windowWidth, windowHeight, SDL_RENDERER_ACCELERATED, &window, &renderer);
		SDL_SetWindowTitle(window, windowTitle.c_str());
	}

	SDL_Event event;
	while (running) {
		while (SDL_PollEvent(&event) != 0) {
			if (event.type == SDL_QUIT) {
				running = false;
			}
		}
	}
	SDL_DestroyRenderer(renderer);
	SDL_DestroyWindow(window);
	SDL_Quit();

	return 0;
}
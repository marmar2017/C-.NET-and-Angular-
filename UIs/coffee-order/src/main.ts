import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideHttpClient } from '@angular/common/http';
import { provideLottieOptions, provideCacheableAnimationLoader } from 'ngx-lottie';
import player from 'lottie-web';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),
    provideLottieOptions({
      player: () => player
    }),
    provideCacheableAnimationLoader()
  ]
}).catch((err) => console.error(err));

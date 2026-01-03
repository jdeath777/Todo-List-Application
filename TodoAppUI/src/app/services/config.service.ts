import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom, of } from 'rxjs';
import { catchError, timeout } from 'rxjs/operators';

export interface AppConfig {
  apiUrl: string;
}

@Injectable({ providedIn: 'root' })
export class ConfigService {
  private config: AppConfig | null = null;

  // A hard-coded fallback to use when config can't be loaded quickly.
  // Set this to your local backend URL.  
  // We default to the same host/port used in `public/config.json`.
  private readonly fallbackConfig: AppConfig = { apiUrl: 'https://localhost:7078/' };

  constructor(private http: HttpClient) {}

  /**
   * Load runtime config from /assets/config.json, but don't hang the app: if
   * the request times out or fails, use a local fallback.
   */
  load(timeoutMs = 3000): Promise<void> {
    // Use a relative path so this works even when the app is hosted under a
    // subpath. The `public/` folder is already listed in `angular.json` assets
    // so `public/config.json` will be available at `/config.json` in the dev
    // server and in production builds.
    return firstValueFrom(
      this.http.get<AppConfig>('config.json').pipe(
        timeout(timeoutMs),
        catchError((err) => {
          // Log the  error, then return fallback.
          console.warn('ConfigService: failed to load config, using fallback', err && err.message ? err.message : err);
          return of(this.fallbackConfig);
        })
      )
    ).then(config => {
      this.config = config;
    });
  }

  /**
   * Return the API URL. If runtime config hasn't loaded yet, return the
   * fallback instead of throwing so callers (services) can proceed.
   */
  get apiUrl(): string {
    if (!this.config) {
      // Warning intentionally non-fatal — caller can still function using fallback.
      console.warn('ConfigService: config not loaded yet, using fallback apiUrl');
      return this.fallbackConfig.apiUrl;
    }
    return this.config.apiUrl;
  }
}

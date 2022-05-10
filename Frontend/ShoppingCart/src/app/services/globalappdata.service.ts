import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

/**
 *  Service that is responsible for sharing global variable data across the application.
 */
@Injectable({
  providedIn: 'root',
})
export class GlobalappdataService {
  apiURL: string = environment.apiServer;

  /**
   * Returns the API URL for each environment dynamically.
   */
  get GetApiUrl(): string {
    return this.apiURL;
  }
}

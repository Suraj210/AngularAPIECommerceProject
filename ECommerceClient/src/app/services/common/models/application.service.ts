import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { Observable, firstValueFrom } from 'rxjs';
import { Menu } from '../../../contracts/application-configurations/menu';

@Injectable({
  providedIn: 'root',
})
export class ApplicationService {
  constructor(private httpClientSerice: HttpClientService) {}

  async getAuthorizeDefinitioonEndpoints(): Promise<Menu[]> {
    const observable: Observable<Menu[]> = this.httpClientSerice.get<Menu[]>({
      controller: 'ApplicationServices',
    });

    return await firstValueFrom(observable);
  }
}

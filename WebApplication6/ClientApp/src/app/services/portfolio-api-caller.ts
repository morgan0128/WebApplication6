import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AlbumItem} from '../models/AlbumInterfacing';
import {CreatePortfolioPageFromAlbumRequest, PortfolioPageItem} from '../models/PortfolioInterfacing';

@Injectable({
  providedIn: 'root',
})
export class PortfolioApiCaller {
  constructor() {
  };

  private readonly http = inject(HttpClient);
  private readonly apiPortfolioUrl = '/api/Portfolio';

  getPageLayoutPresetNumberValues(): (Observable<string[]>) {
    let requestPath = this.apiPortfolioUrl + '/styling-enums';
    return this.http.get<string[]>(requestPath);
  }

  // getPortfolioPageItemForAlbum(albumId: number): (Observable<PortfolioPageItem | null>) {
  //   let requestPath = this.apiPortfolioUrl + '/by-album/' + albumId;
  //   return this.http.get<PortfolioPageItem | null>(requestPath);
  // }

  fetchOrCreatePortfolioPage(albumModel: CreatePortfolioPageFromAlbumRequest): (Observable<PortfolioPageItem | null>){
    let requestPath = this.apiPortfolioUrl + '/fetch-or-create';
    return this.http.post<PortfolioPageItem | null>(requestPath, albumModel);
  }

}

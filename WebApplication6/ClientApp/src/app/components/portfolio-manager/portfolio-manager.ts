import {Component, inject, input, signal, ViewEncapsulation} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {PortfolioApiCaller} from '../../services/portfolio-api-caller';
import {AlbumItem} from '../../models/AlbumInterfacing';
import {CreatePortfolioPageFromAlbumRequest, PortfolioPageItem} from '../../models/PortfolioInterfacing';
import {toObservable, toSignal} from '@angular/core/rxjs-interop';
import {of, switchMap} from 'rxjs';

@Component({
  selector: 'app-portfolio-manager',
  imports: [
    FormsModule
  ],
  templateUrl: './portfolio-manager.html',
  styleUrl: './portfolio-manager.css',
})
export class PortfolioManager {
  private readonly portfolioApi = inject(PortfolioApiCaller);

  public readonly selectedAlbum = input.required<AlbumItem | null>();
  private readonly selectedAlbum$ = toObservable(this.selectedAlbum);

  protected readonly portfolioPage = toSignal(
    this.selectedAlbum$.pipe(
      switchMap(album => {
          if (album == null) {
            return of(null);
          }

          const request = new CreatePortfolioPageFromAlbumRequest(album.id, album.name ?? '');

          return this.portfolioApi.fetchOrCreatePortfolioPage(request);
        })
      ),
      { initialValue: null }
  );

  protected readonly stylingLayouts = toSignal<string[]>(this.portfolioApi.getPageLayoutPresetNumberValues());
  protected selectedStyleLayout: string | null = null;




  onPortfolioPageItemLoaded(){

  }

  // private initializePortfolioPage() : (PortfolioPageItem | null) {
  //   if (this.selectedAlbum == null) return null;
  //   let createRequestItem = new CreatePortfolioPageFromAlbumRequest(this.selectedAlbum()!.id, this.selectedAlbum()!.name ?? '');
  //
  //   this.portfolioApi.postPortfolioPage(createRequestItem).subscribe({
  //     next: item => {
  //       return item;
  //     },
  //     error: () => {
  //       return null;
  //     },
  //     complete: () => {
  //       return null;
  //     }
  //   });
  // }

  onApplyStyling() {
    if (this.selectedStyleLayout == null) return;

  }


}

import {Component, inject, input, linkedSignal, signal, ViewEncapsulation} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {PortfolioApiCaller} from '../../services/portfolio-api-caller';
import {AlbumItem} from '../../models/AlbumInterfacing';
import {
  CreatePortfolioPageFromAlbumRequest,
  PageLayoutPreset,
  PortfolioPageItem
} from '../../models/PortfolioInterfacing';
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

  protected readonly fetchedPortfolioPage = toSignal(
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

  protected readonly portfolioPage = linkedSignal(() => this.fetchedPortfolioPage());

  protected readonly stylingLayouts = toSignal<PageLayoutPreset[]>(this.portfolioApi.getPageLayoutPresets());
  // protected selectedStyleLayout: string | null = null;
  protected selectedStyleLayout: PageLayoutPreset | null = null;




  onPortfolioPageItemLoaded(){
    // TODO select the stylingLayout associated with the portfolio page
  }

  onApplyStyling() {
    if (this.portfolioPage() == null || this.selectedStyleLayout == null) return;
    const portfolio = this.portfolioPage()!;
    const layoutPreset = this.selectedStyleLayout;



    this.portfolioApi.applyPageLayoutPreset(portfolio.id, layoutPreset)
      .subscribe({
        next: () => {
          this.portfolioPage.update(current =>
            current == null ? null : { ...current, layoutPreset }
          );
        }
      });
  }


}

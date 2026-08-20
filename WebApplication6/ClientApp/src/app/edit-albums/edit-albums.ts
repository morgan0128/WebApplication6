import {Component, inject, OnInit, signal, Signal} from '@angular/core';
import {FormsModule} from '@angular/forms';
// import {NgOptimizedImage} from '@angular/common';
import {HttpClient} from '@angular/common/http';

interface AlbumDTO{
  id: number,
  name: string | null,
  description: string | null,
}

@Component({
  selector: 'app-edit-albums',
  imports: [
    FormsModule
  ],
  templateUrl: './edit-albums.html',
  styleUrl: './edit-albums.css',
})
export class EditAlbums implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly apiAlbumUrl = '/api/Album';

  protected readonly creatingAlbum = signal<boolean>(false);
  protected readonly creatingAlbumError = signal<boolean>(false);
  protected newAlbumTitle = '';
  protected newAlbumDescription = '';

  protected readonly loadingAlbums = signal<boolean>(true);
  protected readonly loadingAlbumsError = signal<boolean>(false);

  protected readonly selectedAlbum = signal<AlbumDTO | undefined>(undefined);
  // protected readonly selectedAlbumId = signal<number | null>(null);
  selectedAlbumId: number | null = null;

  protected readonly selectingAlbumError = signal<boolean>(false);



  protected readonly albumDTOs = signal<AlbumDTO[]>([]);

  ngOnInit() {
    this.loadAlbums();
  }

  loadAlbums(){
    this.loadingAlbums.set(true);

    let requestPath = this.apiAlbumUrl + '/all';
    this.http.get<AlbumDTO[]>(requestPath).subscribe({
      next: dtos => {
        this.albumDTOs.set(dtos);
        this.loadingAlbums.set(false);
      },
      error: () => {
        this.loadingAlbums.set(false);
        this.loadingAlbumsError.set(true);
      }
    })
  }

  createAlbum(){
    this.creatingAlbum.set(true);
    this.creatingAlbumError.set(false);
    const name = this.newAlbumTitle.trim();
    const description = this.newAlbumDescription.trim();

    let requestPath = this.apiAlbumUrl + '';
    this.http.post(requestPath, { name, description }).subscribe({
      next: () => {
        this.creatingAlbum.set(false);
        this.loadAlbums();
      },
      error: () => {
        this.creatingAlbumError.set(true);
        this.creatingAlbum.set(false);
      }
    })
  }

  onSelectedAlbumChange(){
    // return;

    if (this.selectedAlbumId == null){
      return; // default selection value
    }
    if (this.selectedAlbum()?.id == this.selectedAlbumId){
      return; // already selected
    }

    this.selectingAlbumError.set(false);

    const albumDTO = this.albumDTOs().find(a => a.id == this.selectedAlbumId);

    if (!albumDTO){
      this.selectingAlbumError.set(true);
      return;
    }

    this.selectedAlbum.set(albumDTO);
    this.loadAlbumSelection();
  }

  loadAlbumSelection(){

  }

}

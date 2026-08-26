// import {Component, inject, OnInit, signal, ViewEncapsulation} from '@angular/core';
// import { AlbumDTO } from '../album-dto';
// import {HttpClient} from '@angular/common/http';
// import {FormsModule} from '@angular/forms';
// import {PhotoDTO} from '../photo-dto';
//
// @Component({
//   selector: 'app-global-album-selector',
//   imports: [
//     FormsModule
//   ],
//   templateUrl: './global-album-selector.html',
//   styleUrl: '../edit-albums/edit-albums.css',
//   encapsulation: ViewEncapsulation.None,
//   // styles: `
//   //   select {
//   //     appearance: auto;
//   //   }
//   // `
// })
// export class GlobalAlbumSelector implements OnInit {
//
//   private readonly http = inject(HttpClient);
//   private readonly apiAlbumUrl = '/api/Album';
//
//   public readonly loadingAlbums = signal<boolean>(true);
//   public readonly loadingAlbumsError = signal<boolean>(false);
//
//   public selectedAlbum: AlbumDTO | null = null;
//   // protected readonly selectedAlbumId = signal<number | null>(null);
//   public selectedAlbumId: number | null = null;
//   public readonly selectingAlbumError = signal<boolean>(false);
//
//   public readonly loadingPhotos = signal<boolean>(true);
//   public readonly loadingPhotosError = signal<boolean>(false);
//
//   public readonly albumDTOs = signal<AlbumDTO[]>([]);
//   public readonly photoDTOs = signal<PhotoDTO[]>([]);
//
//   ngOnInit() {
//     this.loadAlbums();
//   }
//
//   loadAlbums(){
//     this.loadingAlbums.set(true);
//
//     let requestPath = this.apiAlbumUrl + '/all';
//     this.http.get<AlbumDTO[]>(requestPath).subscribe({
//       next: dtos => {
//         this.albumDTOs.set(dtos);
//         this.loadingAlbums.set(false);
//       },
//       error: () => {
//         this.loadingAlbums.set(false);
//         this.loadingAlbumsError.set(true);
//       }
//     })
//   }
//
//   onSelectedAlbumChange(){
//     // return;
//     // this.proposingDelete.set(false);
//
//     if (this.selectedAlbumId == null){
//       this.selectedAlbum = null;
//       this.photoDTOs.set([]);
//       return; // default selection value
//     }
//     if (this.selectedAlbum?.id == this.selectedAlbumId){
//       return; // already selected
//     }
//
//     this.selectingAlbumError.set(false);
//
//     const albumDTO = this.albumDTOs().find(a => a.id == this.selectedAlbumId);
//
//     if (!albumDTO){
//       this.selectingAlbumError.set(true);
//       return;
//     }
//
//     this.selectedAlbum = albumDTO;
//     this.photoDTOs.set([]);
//     this.loadAlbumSelection().then(r => { return; });
//   }
//
//   async loadAlbumSelection(){
//     this.loadingPhotos.set(true);
//     this.loadingPhotosError.set(false);
//     this.photoDTOs.set([]);
//
//     let requestPath = this.apiAlbumUrl + '/' + this.selectedAlbumId + '/photos';
//     this.http.get<PhotoDTO[]>(requestPath).subscribe({
//       next: dtos => {
//         this.photoDTOs.set(dtos);
//       },
//       error: () => {
//         this.loadingPhotos.set(false);
//         this.loadingPhotosError.set(true);
//       },
//       complete: () => {
//         this.loadingPhotos.set(false);
//       }
//     })
//   }
// }

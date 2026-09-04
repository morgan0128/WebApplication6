export type PortfolioPageItem = {
  id: number,
  navTitle: string,
  title: string,
  published: boolean,
  navbarOrder: number,
  albumId: number,
  pageLayoutPreset: string,
}

export class CreatePortfolioPageFromAlbumRequest {
  constructor(albumId: number, name: string){
    this.albumId = albumId;
    this.name = name;
  }

  albumId: number = -1;
  name: string = '';
}

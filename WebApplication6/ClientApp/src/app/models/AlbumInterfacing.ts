export type ImageItem = {
  id: number,
  fileName: string,
  contentType: string,
  fileSize: number | null,
  storageFileName: string,
  url: string,
  altText: string,
  width: number,
  height: number
}

export type PhotoItem = {
  id: number | null,
  name: string | null,
  description: string | null,
  yearContentCreated: number | null,
  image: ImageItem | null,
  order: number | null,
  displaysName: boolean,
  displaysDescription: boolean,
  displaysYearCC: boolean
}

export type AlbumItem = {
  id: number,
  name: string | null,
  description: string | null,
}

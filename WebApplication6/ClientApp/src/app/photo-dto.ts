export class PhotoDTO {



  id: number | null = null;
  name: string | null = null;
  description: string | null = null;
  yearContentCreated: number | null = null;
  image: ImageDTO | null = null;
}

interface ImageDTO {
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

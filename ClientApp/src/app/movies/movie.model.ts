export class Movie {
  id: number;
  title: string;
  description: string;
  genre: number;
  durationInMinutes: string;
  yearOfRelease: number;
  director: string;
  dateAdded: Date;
  rating: number;
  watched: boolean
}

export enum Genre {
  Action = 0,
  Comedy = 1,
  Horror = 2,
  Thriller = 3
}

namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);
            Console.WriteLine(ExportAlbumsInfo(context, 9));
            //Test your solutions here
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albumsInfo = context.Producers
                .FirstOrDefault(x => x.Id == producerId)
                .Albums
                .Select(album => new
                {
                    albumName = album.Name,
                    releaseDate = album.ReleaseDate,
                    producerName = album.Producer.Name,
                    songs = album.Songs.Select(song => new
                    {
                        songName = song.Name,
                        price = song.Price,
                        writer = song.Writer.Name
                    })
                    .OrderByDescending(x => x.songName)
                    .ThenBy(x => x.writer)
                    .ToList(),
                    totalAlbumPrice = album.Price
                })
                .OrderByDescending(x => x.totalAlbumPrice)
                .ToList();


            
            foreach (var album in albumsInfo)
            {
                sb.AppendLine($"-AlbumName: {album.albumName}").
                    AppendLine($"-ReleaseDate: {album.releaseDate.ToString("MM/dd/yyyy")}").
                    AppendLine($"-ProducerName: {album.producerName}").
                    AppendLine($"-Songs:");

                int counter = 1;
                foreach (var songs in album.songs)
                {
                    sb.AppendLine($"---#{counter++}").
                        AppendLine($"---SongName: {songs.songName}")
                        .AppendLine($"---Price: {songs.price:f2}")
                        .AppendLine($"---Writer: {songs.writer}");
                }
                sb.AppendLine($"-AlbumPrice: {album.totalAlbumPrice:f2}");
            }   

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}

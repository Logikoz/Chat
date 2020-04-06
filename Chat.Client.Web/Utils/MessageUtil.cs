using System;

namespace Chat.Client.Web.Utils
{
    public class MessageUtil<TAuthor, TMessage>
    {
        private TAuthor[] authors = new TAuthor[10000];
        private TMessage[] messages = new TMessage[10000];
        private uint currentIndex;

        public TAuthor Author { get; set; }
        public TMessage Message { get; set; }

        public void Add(TAuthor author, TMessage message)
        {
            if (currentIndex >= authors.Length)
                throw new IndexOutOfRangeException();

            authors[currentIndex] = author;
            messages[currentIndex] = message;
            currentIndex++;
        }
    }
}
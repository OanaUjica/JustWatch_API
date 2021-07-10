using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1_.NET.ViewModels
{
    public class PaginatedResultSet<TEntity>
    {
        public List<int> FirstPages { get; set; }
        public List<int> LastPages { get; set; }
        public List<int> PreviousPages { get; set; }
        public List<int> NextPages { get; set; }

        public int TotalEntities { get; set; }

        public List<TEntity> Entities { get; set; }

        /*
         * pagina curent:           currentPage
         * entitati per pagina:     perPage                         10          10          10          10          10
         * nr total de entitati:    totalEntities                   5           10          12          20          21
         * ultima pagina =          totalEntities / perPage         0           1           1           2           2
         *                                                          1           1           2           2           3
         *                                                          
         *                                                          
         *                          totalEntities / perPage + (totalEntities % perPage > 0) - OK!
         *                                                          1           1           2           2           3
         *                                                          1           1           2           2           3
         */

        public PaginatedResultSet(
            List<TEntity> entities,
            int currentPage, int totalEntities,
            int perPage = 20,
            int numFirstPages = 5, int numLastPages = 5, int numPreviousPages = 6, int numNextPages = 6)
        {
            this.Entities = entities;
            this.FirstPages = new List<int>();
            this.LastPages = new List<int>();
            this.PreviousPages = new List<int>();
            this.NextPages = new List<int>();
            this.TotalEntities = totalEntities;

            int lastPage = totalEntities / perPage + (totalEntities % perPage > 0 ? 1 : 0);
            /* i++:
             * int tempi = i;
             * i = i + 1;
             * use tempi
             * 
             * 
             * ++i:
             * i = i + 1;
             * use i
             */
            for (int i = 0; i < Math.Min(currentPage - 1, numFirstPages); ++i)
            {
                this.FirstPages.Add(i + 1);
            }

            /**
             * currentPage:         97
             * lastPage:            101
             * numLastPages:        5
             * firstOfLast:         97
             * countLast:           101 - 97 + 1 = 5
             */

            int firstOfLast = lastPage - numLastPages + 1;
            if (firstOfLast <= currentPage)
            {
                firstOfLast = currentPage + 1;
            }
            int countLast = lastPage - firstOfLast + 1;
            for (int i = 0; i < countLast; ++i)
            {
                this.LastPages.Add(firstOfLast + i);
            }

            for (int i = 0; i < numPreviousPages; ++i)
            {
                int value = currentPage - numPreviousPages + i;
                if (value > 0 && value < currentPage && !FirstPages.Contains(value))
                {
                    this.PreviousPages.Add(value);
                }
            }
            for (int i = 0; i < numNextPages; ++i)
            {
                int value = currentPage + i + 1;
                if (value <= lastPage && !LastPages.Contains(value))
                {
                    this.NextPages.Add(value);
                }
            }
        }
    }
}

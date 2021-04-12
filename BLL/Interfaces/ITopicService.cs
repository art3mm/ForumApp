using BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface ITopicService : ICrud<TopicModel>
    {
        IEnumerable<TopicModel> FindTopicsByUserNickName(string userNikName);
        IEnumerable<TopicModel> FindTopicsByTitleKeyWord(string titleKeyWord);
        IEnumerable<PopularityTopicModel> GetMostPopularTopicsNUmber(int topicCount);
    }

}
